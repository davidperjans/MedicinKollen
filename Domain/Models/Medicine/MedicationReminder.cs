using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Medicine
{
    public class MedicationReminder
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Medicine Medicine { get; set; }
        public DateTime ReminderTime { get; set; }
        public TimeSpan Frequency { get; set; }

        public MedicationReminder() { }

        public MedicationReminder(Medicine medicine, DateTime reminderTime, TimeSpan frequency, Guid userId)
        {
            Medicine = medicine;
            ReminderTime = reminderTime;
            Frequency = frequency;
            UserId = userId;
        }

        public bool IsReminderTime(DateTime currentTime)
        {
            var timeDifference = currentTime - ReminderTime;

            return timeDifference.TotalMinutes >= 0 && (timeDifference.TotalMinutes % Frequency.TotalMinutes) == 0;
        }

    }
}
