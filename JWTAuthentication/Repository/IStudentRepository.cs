using JWTAuthentication.Models;

namespace JWTAuthentication.Repository
{
    public interface IStudentRepository
    {
       Task<List<Student>> GetAllAsync();
       Task<Student?> GetByIdAsync(int id);
        Task<Student> CreateAsync(Student student);    
        Task<Student?> UpdateAsync(int id, Student region);
        Task<Student?> DeleteAsync(int id);
    }
}
