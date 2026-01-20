using AzureBLOBsample.Data;

namespace AzureBLOBsample.Services
{
    public interface ITablestorageService
    {
        Task<AttendeeEntity> GetAttendee(string Profession, string id);
        Task<List<AttendeeEntity>> GetAttendees();

        Task AddAttendee(AttendeeEntity attend);

        Task DeleteAttendee(string Profession, string id);
    }
}
