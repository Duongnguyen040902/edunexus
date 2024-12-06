using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using sep.backend.v1.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Services
{
    public class SchoolService : BaseService<SchoolDTO, School>, ISchoolService
    {
        public SchoolService(IUnitOfWork unitOfWork, IAutoMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<SchoolInfoDTO> GetSchoolById(int id)
        {
            var result = await _unitOfWork.GetRepository<School>().GetSingleByCondition(x => x.Id == id);
            if (result is null)
            {
                var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "thông tin trường học!" }
                            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            var resultDTO = _mapper.Map<School, SchoolInfoDTO>(result);

            return resultDTO;
        }

        public async Task<bool> UpdateInfoSchool(UpdateInfoSchoolDTO updateSchoolDTO)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                School school = await _unitOfWork.GetRepository<School>().GetById(updateSchoolDTO.Id);
                if (school is null)
                {
                    var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "thông tin trường học!" }
                            };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                _mapper.Map<UpdateInfoSchoolDTO, School>(updateSchoolDTO, school);
                school.UpdatedDate = school.CreatedDate;  
                if (updateSchoolDTO.ImageFile != null && updateSchoolDTO.ImageFile.Length > 0)
                {
                    var uploadHelper = new FileUploadHelper();
                    school.Image = await uploadHelper.UploadFile(updateSchoolDTO.ImageFile, "schools");
                }                     
                var result = await _unitOfWork.GetRepository<School>().Update(school);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }     
        }
    }
}
