export class KResultValidate{
    sucessed: boolean = false;
    mensage: string = "";
}

export class Validation {

    /**
     * Valid Schema
     */
    public ValidSchema(schema: ValidationSchema[], json: any): KResultValidate {
        let retornValidate = new KResultValidate();

        retornValidate.sucessed = true;
        retornValidate.mensage = "";

        schema = schema.reverse()

        schema.forEach((validador: ValidationSchema) => {
            
            let value = json[validador.name];
            
            if (value) {
                
                if (typeof value !== validador.type) {
                    retornValidate.sucessed = false;
                    retornValidate.mensage = "Tipo do campo " + validador.textName + " inválido!";
                    return retornValidate;
                }

                if (validador.required && !value) {
                    retornValidate.sucessed = false;
                    retornValidate.mensage = "Campo " + validador.textName + " é obrigatório!";
                    return retornValidate;
                }

                if (validador.type == "string" && validador.maxLength > 0 && (value + '').length > validador.maxLength) {
                    retornValidate.sucessed = false;
                    retornValidate.mensage = "Campo " + validador.textName + " ultrapassou o tamanho permitido para o campo!";
                    return retornValidate;
                }
                if (validador.valid) {
                    let result = validador.valid(value);
                    if (!result.sucessed) {
                        retornValidate.sucessed = false;
                        retornValidate.mensage = result.mensage;
                        return retornValidate;
                    }
                }

               
            } else {

                if (validador.required) {
                    retornValidate.sucessed = false;
                    retornValidate.mensage = "Campo " + validador.textName + " é obrigatório!";
                    return retornValidate;
                }
            }
        })


        return retornValidate;
    }
}

export class ValidationSchema {
    name: string
    textName: string
    type: string
    required: boolean
    maxLength: number
    valid?: (item: any) => (KResultValidate)

    constructor(name: string, textName: string, type: string = "string", required: boolean = false, maxLength: number = 0, valid?: (item: any) => KResultValidate) {
        this.name = name
        this.textName = textName
        this.type = type
        this.required = required
        this.maxLength = maxLength
        this.valid = valid
    }
}