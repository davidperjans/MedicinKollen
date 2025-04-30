using Application.Interfaces;
using Application.Services;
using Domain.Models.Medicine;
using Domain.Models.ResultTypes;
using Domain.Models.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.MedicineCommands.AddMedicationReminder
{
    public class AddMedicationReminderHandler : IRequestHandler<AddMedicationReminderCommand, OperationResult<MedicationReminder>>
    {
        private readonly IGenericRepository<User> _userGenericRepository;
        private readonly IGenericRepository<Medicine> _medicineRepository;
        private readonly UserService _userService;
        public AddMedicationReminderHandler(IGenericRepository<Medicine> medicineRepository, UserService userService, IGenericRepository<User> userGenericRepository)
        {
            _medicineRepository = medicineRepository;
            _userService = userService;
            _userGenericRepository = userGenericRepository;
        }
        public async Task<OperationResult<MedicationReminder>> Handle(AddMedicationReminderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _userService.GetUserIdFromToken();
                var user = await _userGenericRepository.GetByIdAsync(userId);

                if (user == null)
                    return OperationResult<MedicationReminder>.Failure("User not found");

                var medicine = await _medicineRepository.GetByIdAsync(request.MedicineId);
                if (medicine == null)
                    return OperationResult<MedicationReminder>.Failure("Medicine not found");

                var reminder = new MedicationReminder(medicine.Data, request.ReminderTime, request.Frequency, userId);
                user.Data.MedicationReminders.Add(reminder);

                await _userGenericRepository.UpdateAsync(user.Data);

                return OperationResult<MedicationReminder>.Success(reminder);
            }
            catch (Exception ex)
            {
                return OperationResult<MedicationReminder>.Failure(ex.Message);
            }
        }
    }
}
