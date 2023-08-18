import { IAccount } from "@/pages/Administration/Account/Type/IAccount"
import { IRevenueCategory } from "@/pages/Administration/RevenueCategory/Type/IRevenueCategory"
import { ITypePerson } from "@/pages/Administration/TypePerson/Type/ITypePerson"



export class IRevenue {
    id: number = 0
    value: number = 0
    description: string = ""
    obs: string = ""
    revenueReceipt: string = "Não Especificado" 
    revenueStatus: string = "Recebido" 
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()
    idUser: number = 0;
    idRevenueCategory: number = 0;
    idAccount: number = 0;
    idTypePerson: number = 0;

    revenueCategory: IRevenueCategory = new IRevenueCategory
    account:  IAccount = new IAccount
    supplier: ITypePerson = new ITypePerson

    totalSumRevenue: number = 0
}

