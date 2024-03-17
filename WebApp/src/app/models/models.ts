export class User{
  email: string;
  isAdmin: boolean;
}

export enum ScrapeRequestType{
  Immediate = "Immediate",
  Timed = "Timed"
}

export class ScrapeRequest{
  RequestType: ScrapeRequestType;
  Days: number;
}


