using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace UPS_Event_Log_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UPSController : ControllerBase
    {
        private readonly ILogger<UPSController> _logger;
        private int powerFlag = 0;
        private int commFlag = 0;
        private DateTime powerFlagTime = DateTime.MinValue;
        private DateTime commFlagTime = DateTime.MinValue;
        public DateTime sinceTime = DateTime.Now.AddDays(-7);

        public UPSController(ILogger<UPSController> logger)
        {
            _logger = logger;
        }

        [HttpGet("upsStatus")]
        public IActionResult GetUPSStatus()
        {
            EventLog eventLog = new EventLog("Application");

            var query = from EventLogEntry entry in eventLog.Entries
                        where (entry.EntryType == EventLogEntryType.Information || entry.EntryType == EventLogEntryType.Warning)
                              && entry.TimeGenerated >= sinceTime
                        select new
                        {
                            TimeGenerated = entry.TimeGenerated,
                            Message = entry.Message
                        };

            var eventList = query.ToList();

            foreach (var entry in eventList)
            {
                if (entry.Message.Contains("Utility power failure"))
                {
                    powerFlag = 1;
                    powerFlagTime = entry.TimeGenerated.ToUniversalTime();
                }
                else if (entry.Message.Contains("Utility power has been restored"))
                {
                    powerFlag = 0;
                    powerFlagTime = entry.TimeGenerated.ToUniversalTime();
                }
                else if (entry.Message.Contains("Local communication with the device has been lost"))
                {
                    commFlag = 1;
                    commFlagTime = entry.TimeGenerated.ToUniversalTime();
                }
                else if (entry.Message.Contains("Communication with the device has resumed"))
                {
                    commFlag = 0;
                    commFlagTime = entry.TimeGenerated.ToUniversalTime();
                }
            }
            // If there are no power messages, set powerFlag to 0 (good)
            if (!eventList.Any(entry => entry.Message.Contains("Utility power failure") || entry.Message.Contains("Utility power has been restored")))
            {
                powerFlag = 0;
                powerFlagTime = DateTime.UtcNow;
            }

            // If there are no communication messages, set commFlag to 0 (good)
            if (!eventList.Any(entry => entry.Message.Contains("Local communication with the device has been lost") || entry.Message.Contains("Communication with the device has resumed")))
            {
                commFlag = 0;
                commFlagTime = DateTime.UtcNow;
            }
            var jsonPayload = new
            {
                CurrentTimeUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                //CurrentTimeUTC = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                PowerFlag = powerFlag,
                CommFlag = commFlag,
                PowerFlagTime = powerFlagTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                CommFlagTime = commFlagTime.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            return Ok(jsonPayload);
        }

        [HttpGet("upsData")]
        public IActionResult GetUPSData()
        {
            EventLog eventLog = new EventLog("Application");

            var query = from EventLogEntry entry in eventLog.Entries
                        where (entry.EntryType == EventLogEntryType.Information || entry.EntryType == EventLogEntryType.Warning)
                              && entry.EventID == 2485
                              && entry.TimeGenerated >= sinceTime
                        select new
                        {
                            TimeGenerated = entry.TimeGenerated,
                            Message = entry.Message
                        };

            var eventList = query.ToList();
            // If there are no messages, return an empty list
            if (eventList.Count == 0)
            {
                return Ok(new List<object>());
            }
            return Ok(eventList);
        }
    }
}
