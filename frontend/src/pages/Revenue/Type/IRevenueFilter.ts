import { IAccount } from "@/pages/Administration/Account/Type/IAccount"
import { IRevenueCategory } from "@/pages/Administration/RevenueCategory/Type/IRevenueCategory"
import { ITypePerson } from "@/pages/Administration/TypePerson/Type/ITypePerson"



export class IRevenueFilter {

    dateInitial: Date | null = new Date()
    dateFinal: Date | null = new Date()
    revenueReceipt: string = "";
    revenueStatus: string = "Todas";

    revenueCategory: IRevenueCategory | null = new IRevenueCategory
    account:  IAccount | null= new IAccount
    supplier: ITypePerson | null = new ITypePerson
}

