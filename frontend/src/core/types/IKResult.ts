export class IResult {
    data: IData = {} as IData
}

export class IData{
    sucesso: boolean = false;
    errors: any;
    message: string = "";
    tRetorno: any;
}
