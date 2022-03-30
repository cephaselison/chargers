export interface InstallationDto {
  id: string,
  name: string,
  circuitBreakerCurrent: number,
  circuitBreakerTripped: boolean,
  chargers: ChargerDto[],
}

export interface ChargerDto {
  id: string,
  name: string,
  installationId: string,
  connectedVehicle: VehicleDto | null,
  allocatedCurrent: number,
  connected: boolean | null;
}

export interface VehicleDto {
  batteryCapacityWh: number,
  charging: boolean,
  chargeCurrent: number,
  availableCurrent: number,
  batteryChargeWh: number
}