import { IAccountCategory } from "../../AccountCategory/Type/IAccountCategory"
import { ITypePerson } from "../../TypePerson/Type/ITypePerson"

export class IAccount {
    id: number = 0
    name: string = ""
    agency: string = ""
    accountNumber: string = ""
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()
    idUser: number = 0;
    idAccountCategory: number = 0;
    idTypePerson: number = 0;

    accountCategory:  IAccountCategory = new IAccountCategory
    supplier: ITypePerson = new ITypePerson
}

