using ChoiceVSharedApiModels.Vehicles;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IVehicleApi : IBaseApiInterface
{
    [Get("/api/v1/vehicle")]
    Task<ApiResponse<List<VehicleApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/vehicle?vehicleId={vehicleId}")]
    Task<ApiResponse<VehicleApiModel?>> GetByVehicleIdAsync(int vehicleId); 
    
    [Get("/api/v1/vehicle?characterId={characterId}")]
    Task<ApiResponse<List<VehicleApiModel>>> GetAllByCharacterIdAsync(int characterId); 
    
    [Get("/api/v1/vehicle?companyId={companyId}")]
    Task<ApiResponse<List<VehicleApiModel>>> GetAllByCompanyIdAsync(int companyId); 
    
    [Get("/api/v1/vehicle/configs")]
    Task<ApiResponse<List<ConfigVehicleApiModel>>> GetAllConfigAsync(); 
    
}