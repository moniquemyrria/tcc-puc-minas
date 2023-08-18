import { IAccount } from "@/pages/Administration/Account/Type/IAccount"
import { IExpenseCategory } from "@/pages/Administration/ExpenseCategory/Type/IExpenseCategory"
import { ITypePerson } from "@/pages/Administration/TypePerson/Type/ITypePerson"



export class IExpenseFilter {

    dateInitial: Date | null = new Date()
    dateFinal: Date | null = new Date()
    expenseStatus: string = "Todas";

    expenseCategory: IExpenseCategory | null = new IExpenseCategory
    account:  IAccount | null= new IAccount
    supplier: ITypePerson | null = new ITypePerson
}

