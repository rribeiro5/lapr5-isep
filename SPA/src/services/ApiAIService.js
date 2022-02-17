import apiAI from "./apiAI";

export const getSafestTrajectUnidirecional  = (userOrig,userDest,minValue) => {
    return apiAI.get(`/api/caminhoSeguroUnidirecional?origId=${userOrig}&destId=${userDest}&minLigacao=${minValue}`)
}

export const getSafestTrajectBidirecional  = (userOrig,userDest,minValue) => {
    return apiAI.get(`/api/caminhoSeguroBidirecional?origId=${userOrig}&destId=${userDest}&minLigacao=${minValue}`)
}

export const getStrongestTrajectUnidirecional = (userOrig,userDest) => {
    return apiAI.get(`/api/caminhoMaisForcaUnidirecional?orig=${userOrig}&dest=${userDest}`)
}

export const getStrongestTrajectBidirecional = (userOrig,userDest) => {
    return apiAI.get(`/api/caminhoMaisForcaBidirecional?orig=${userOrig}&dest=${userDest}`)
}

export const getSuggestedUser = (userId) => {
    return apiAI.get(`/api/suggested?id=${userId}`)
}

export const getShortestTraject = (userOrig,userDest) => {
    return apiAI.get(`/api/caminhoMaisCurto?origId=${userOrig}&destId=${userDest}`)
}

export const getDFSFLig  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoDFSFLigacao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getDFSFRel  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoDFSFRelacao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getBestFirstFLig  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoBestfsFLig?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getBestFirstFRel  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoBestfsFLigFRel?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getAStarFLig  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoAStarFLig?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getAStarFRel  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoAStarFLigFRel?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getDFSFLigEmotion  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoDFSFLigacaoEmocao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getDFSFRelEmotion  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoDFSFLigFRelEmocao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getBestFirstFLigEmotion  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoBestfsFLigEmocao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getBestFirstFRelEmotion  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoBestfsFLigFRelEmocao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getAStarFLigEmotion  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoAStarFLigEmocao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}

export const getAStarFRelEmotion  = (userOrig,userDest,nLimit) => {
    return apiAI.get(`/api/caminhoAStarFLigFRelEmocao?orig=${userOrig}&dest=${userDest}&maxLigacoes=${nLimit}`)
}
