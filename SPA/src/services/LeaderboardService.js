import apiMDRS from "./apiMDRS"


export const leaderboardNetworkDimension = () => apiMDRS.get("/api/leaderboard/networkDimension")

export const leaderboardNetworkStronghold = () => apiMDRS.get("/api/leaderboard/networkStrength")
