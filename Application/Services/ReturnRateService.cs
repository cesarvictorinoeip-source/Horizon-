using Persistence.Entities;
using Persistence.Repositories;
using Application.Dtos.ReturnRate;

namespace Application.Services
{
    public class ReturnRateService
    {
        private readonly ReturnRateRepository _repository;

        public ReturnRateService(ReturnRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReturnRateDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Select(e => new ReturnRateDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    MinRate = e.MinRate,
                    MaxRate = e.MaxRate
                }).ToList();
            }
            catch (Exception)
            {
                return new List<ReturnRateDto>();
            }
        }

        public async Task<ReturnRateDto?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return null;

                return new ReturnRateDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    MinRate = entity.MinRate,
                    MaxRate = entity.MaxRate
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ReturnRateDto?> AddAsync(ReturnRateDto dto)
        {
            // --- VALIDACIÓN DE MANDATO INCLUIDA ---
            if (dto.MinRate >= dto.MaxRate) return null;

            try
            {
                var entity = new ReturnRateConfig
                {
                    Id = dto.Id,
                    Name = dto.Name ?? "Configuración General",
                    MinRate = dto.MinRate,
                    MaxRate = dto.MaxRate
                };

                var result = await _repository.AddAsync(entity);
                if (result == null) return null;

                dto.Id = result.Id;
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ReturnRateDto?> UpdateAsync(int id, ReturnRateDto dto)
        {
            // --- VALIDACIÓN DE MANDATO INCLUIDA ---
            if (dto.MinRate >= dto.MaxRate) return null;

            try
            {
                var entity = new ReturnRateConfig
                {
                    Id = id,
                    Name = dto.Name ?? "Configuración General",
                    MinRate = dto.MinRate,
                    MaxRate = dto.MaxRate
                };

                var result = await _repository.UpdateAsync(id, entity);
                if (result == null) return null;

                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}