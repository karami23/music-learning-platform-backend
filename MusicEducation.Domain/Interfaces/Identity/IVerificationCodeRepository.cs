using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;

namespace MusicEducation.Domain.Interfaces.Identity;

public interface IVerificationCodeRepository
{
    Task<VerificationCode?> GetActiveByUserIdAndPurposeAsync(
        int userId,
        VerificationCodePurpose purpose);

    Task<VerificationCode> AddAsync(VerificationCode verificationCode);

    Task UpdateAsync(VerificationCode verificationCode);
}