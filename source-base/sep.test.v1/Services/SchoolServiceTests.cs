using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IRepositories;
using sep.backend.v1.Services.UnitOfWork;
using sep.test.v1.Helper;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace sep.test.v1.Services
{
    public class SchoolServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ISchoolRepository> _mockSchoolRepository;
        private readonly Mock<IAutoMapper> _mockAutoMapper;
        private readonly SchoolService _schoolService;
        private readonly Mock<IFileUploadHelper> _fileUploadHelper;
        private List<SchoolDTO> schoolDTOs;

        public SchoolServiceTests()
        {
            // Setup mock services and controller      
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAutoMapper = new Mock<IAutoMapper>();
            _mockSchoolRepository = new Mock<ISchoolRepository>();
            _fileUploadHelper = new Mock<IFileUploadHelper>();
            _schoolService = new SchoolService(_mockUnitOfWork.Object, _mockAutoMapper.Object);
            //option
            schoolDTOs = new List<SchoolDTO>()
            {
                //add property to using
                new SchoolDTO(){Id= 1,SchoolName="ABC"}
            };
        }

        [Fact]
        public async Task GetInfoSchool_WithValidId_ReturnsExpectedSchool()
        {
            // Arrange
            int schoolId = 1;

            // Mock school entity
            var schoolEntity = new School
            {
                Id = schoolId,
                Name = "Greenwood High",
                Address = "Tổ 6",
                Province = "30",
                District = "299",
                Ward = "11230",
                Email = "admin@greenwoodhigh.edu",
                PhoneNumber = "0383876125",
                PrincipalName = "John Doe",
                PrincipalPhone = "0383876125",
                WebsiteLink = "http://greenwoodhigh.edu",
                Image = "Resources/schools/a2713dfb-ba87-429f-813d-ad56f23a2bfd_11.png",
                StandardCode = "GH123",
                DateOfEstablishment = new DateTime(1990, 1, 1),
                FAX = "123456789"
            };
            var expectedSchool = new SchoolInfoDTO
            {
                Id = schoolId,
                Name = "Greenwood High",
                Address = "Tổ 6",
                Province = "30",
                District = "299",
                Ward = "11230",
                Email = "admin@greenwoodhigh.edu",
                PhoneNumber = "0383876125",
                PrincipalName = "John Doe",
                PrincipalPhone = "0383876125",
                WebsiteLink = "http://greenwoodhigh.edu",
                Image = "Resources/schools/a2713dfb-ba87-429f-813d-ad56f23a2bfd_11.png",
                StandardCode = "GH123",
                DateOfEstablishment = new DateTime(1990, 1, 1),
                FAX = "123456789"
            };
            //// Mock repository to return the school entity
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().GetSingleByCondition(x => x.Id == schoolId, null))
                .ReturnsAsync(schoolEntity);
            _mockAutoMapper
                .Setup(mapper => mapper.Map<School, SchoolInfoDTO>(schoolEntity))
                .Returns(expectedSchool);
            // Act
            var result = await _schoolService.GetSchoolById(schoolId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedSchool);
        }

        [Fact]
        public async Task GetInfoSchool_NotExist_ThrowsNotFoundException()
        {
            // Arrange
            int schoolId = 10;

            // Giả lập repository trả về null (không tìm thấy trường)
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().GetSingleByCondition(x => x.Id == schoolId, null))
                .ReturnsAsync((School)null);

            var expectedMessage = "Không tìm thấy thông tin trường học!"; // Điều chỉnh theo thông báo thực tế của bạn

            // Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _schoolService.GetSchoolById(schoolId));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedMessage);
        }

        public async Task Update_WithValid_ReturnsOk()
        {
            // Arrange
            int schoolId = 1;

            // Mock school entity
            var schoolEntity = new School
            {
                Id = schoolId,
                Name = "Greenwood High",
                Address = "Tổ 6",
                Province = "30",
                District = "299",
                Ward = "11230",
                Email = "admin@greenwoodhigh.edu",
                PhoneNumber = "0383876125",
                PrincipalName = "John Doe",
                PrincipalPhone = "0383876125",
                WebsiteLink = "http://greenwoodhigh.edu",
                Image = "Resources/schools/a2713dfb-ba87-429f-813d-ad56f23a2bfd_11.png",
                StandardCode = "GH123",
                DateOfEstablishment = new DateTime(1990, 1, 1),
                FAX = "123456789"
            };
            var expectedSchool = new SchoolInfoDTO
            {
                Id = schoolId,
                Name = "Greenwood High",
                Address = "Tổ 6",
                Province = "30",
                District = "299",
                Ward = "11230",
                Email = "admin@greenwoodhigh.edu",
                PhoneNumber = "0383876125",
                PrincipalName = "John Doe",
                PrincipalPhone = "0383876125",
                WebsiteLink = "http://greenwoodhigh.edu",
                Image = "Resources/schools/a2713dfb-ba87-429f-813d-ad56f23a2bfd_11.png",
                StandardCode = "GH123",
                DateOfEstablishment = new DateTime(1990, 1, 1),
                FAX = "123456789"
            };
            //// Mock repository to return the school entity
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().GetSingleByCondition(x => x.Id == schoolId, null))
                .ReturnsAsync(schoolEntity);
            _mockAutoMapper
                .Setup(mapper => mapper.Map<School, SchoolInfoDTO>(schoolEntity))
                .Returns(expectedSchool);
            // Act
            var result = await _schoolService.GetSchoolById(schoolId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedSchool);
        }

        [Fact]
        public async Task UpdateInfoSchool_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            var updateSchoolDTO = new UpdateInfoSchoolDTO
            {
                Id = 1,
                Name = "Updated School",
                Address = "Updated Address",
                Province = "Updated Province",
                District = "Updated District",
                Ward = "Updated Ward",
                Email = "updated@school.com",
                PhoneNumber = "0123456789",
                PrincipalName = "Updated Principal",
                PrincipalPhone = "9876543210",
                WebsiteLink = "http://updatedschool.com",
                StandardCode = "US123",
                DateOfEstablishment = new DateTime(2000, 1, 1),
                FAX = "9876543210",
                ImageFile = null
            };

            var existingSchool = new School
            {
                Id = 1,
                Name = "Old School",
                Address = "Old Address",
                Province = "Old Province",
                District = "Old District",
                Ward = "Old Ward",
                Email = "old@school.com",
                PhoneNumber = "0987654321",
                PrincipalName = "Old Principal",
                PrincipalPhone = "1234567890",
                WebsiteLink = "http://oldschool.com",
                StandardCode = "OS123",
                DateOfEstablishment = new DateTime(1990, 1, 1),
                FAX = "1234567890"
            };

            // Mock repository to return the existing school
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().GetById(updateSchoolDTO.Id))
                .ReturnsAsync(existingSchool);

            // Mock AutoMapper to map DTO to School entity
            _mockAutoMapper
                .Setup(mapper => mapper.Map<UpdateInfoSchoolDTO, School>(updateSchoolDTO, existingSchool));

            // Mock the repository Update method to return true
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().Update(It.IsAny<School>()))
                .ReturnsAsync(true);

            // Mock commit and complete operations for the unit of work
            _mockUnitOfWork
                .Setup(uow => uow.CommitTransactionAsync())
                .Returns(Task.CompletedTask);
            _mockUnitOfWork
                .Setup(uow => uow.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _schoolService.UpdateInfoSchool(updateSchoolDTO);

            // Assert
            result.Should().BeTrue();  // Assert that the result is true, indicating successful update
        }

        [Fact]
        public async Task UpdateInfoSchool_SchoolNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var updateSchoolDTO = new UpdateInfoSchoolDTO
            {
                Id = 10, // ID does not exist in the system
                Name = "Updated School",
                Address = "Updated Address"
            };

            // Mock repository to return null (no school found)
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().GetById(updateSchoolDTO.Id))
                .ReturnsAsync((School)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _schoolService.UpdateInfoSchool(updateSchoolDTO));
        }

        [Fact]
        public async Task UpdateInfoSchool_WithImageFile_UpdatesSuccessfully()
        {
            // Arrange
            var updateSchoolDTO = new UpdateInfoSchoolDTO
            {
                Id = 1,
                Name = "Updated School with Image",
                Address = "Updated Address",
                ImageFile = new Mock<IFormFile>().Object // Mock an image file
            };

            var existingSchool = new School
            {
                Id = 1,
                Name = "Old School",
                Address = "Old Address",
                Province = "Old Province",
                District = "Old District",
                Ward = "Old Ward",
                Email = "old@school.com",
                PhoneNumber = "0987654321",
                PrincipalName = "Old Principal",
                PrincipalPhone = "1234567890",
                WebsiteLink = "http://oldschool.com",
                StandardCode = "OS123",
                DateOfEstablishment = new DateTime(1990, 1, 1),
                FAX = "1234567890"
            };

            // Mock repository to return the existing school
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().GetById(updateSchoolDTO.Id))
                .ReturnsAsync(existingSchool);

            // Mock AutoMapper to map DTO to School entity
            _mockAutoMapper
                .Setup(mapper => mapper.Map<UpdateInfoSchoolDTO, School>(updateSchoolDTO, existingSchool));

            // Mock the repository Update method to return true
            _mockUnitOfWork
                .Setup(repo => repo.GetRepository<School>().Update(It.IsAny<School>()))
                .ReturnsAsync(true);

            // Mock file upload helper to return a file path
            _fileUploadHelper.Setup(x => x.UploadFile(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("new-image-path.jpg");

            // Mock commit and complete operations for the unit of work
            _mockUnitOfWork
                .Setup(uow => uow.CommitTransactionAsync())
                .Returns(Task.CompletedTask);
            _mockUnitOfWork
                .Setup(uow => uow.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _schoolService.UpdateInfoSchool(updateSchoolDTO);

            // Assert
            result.Should().BeTrue();  // Assert that the result is true, indicating successful update
        }
    }
}
