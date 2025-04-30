using Domain.Models.Medicine;
using Domain.Models.ResultTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.MedicineCommands.AddMedicationReminder
{
    public class AddMedicationReminderCommand : IRequest<OperationResult<MedicationReminder>>
    {
        public Guid MedicineId { get; set; }
        public DateTime ReminderTime { get; set; }
        public TimeSpan Frequency { get; set; }
    }
}
