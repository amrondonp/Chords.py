const dev = {
    serverPrefixUrl: "https://localhost:44392"
}

const prod = {
    serverPrefixUrl: "", 
}

const env =  process.env.REACT_APP_ENV === "dev" ? dev : prod;

export function url(resource: string): string {
    return env.serverPrefixUrl + resource;
}