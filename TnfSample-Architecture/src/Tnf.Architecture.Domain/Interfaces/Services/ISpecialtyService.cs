﻿using Tnf.App.Dto.Request;
using Tnf.App.Dto.Response;
using Tnf.Architecture.Dto.Registration;
using Tnf.Domain.Services;

namespace Tnf.Architecture.Domain.Interfaces.Services
{
    public interface ISpecialtyService : IDomainService
    {
        ListDto<SpecialtyDto> GetAllSpecialties(GetAllSpecialtiesDto request);
        SpecialtyDto GetSpecialty(RequestDto<int> requestDto);
        void DeleteSpecialty(int id);
        SpecialtyDto CreateSpecialty(SpecialtyDto dto);
        SpecialtyDto UpdateSpecialty(SpecialtyDto dto);
    }
}
