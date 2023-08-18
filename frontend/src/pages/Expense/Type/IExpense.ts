import { IAccount } from "@/pages/Administration/Account/Type/IAccount"
import { IExpenseCategory } from "@/pages/Administration/ExpenseCategory/Type/IExpenseCategory"
import { IPaymentMethods } from "@/pages/Administration/PaymentMethods/Type/IPaymentMethods"
import { IPaymentMethodsTypes } from "@/pages/Administration/PaymentMethods/Type/IPaymentMethodsTypes"
import { ITypePerson } from "@/pages/Administration/TypePerson/Type/ITypePerson"

export class IExpense {
    id: number = 0
    description: string = ""
    obs: string = ""
    expenseStatus = "A Pagar" 
    amount: number | null = null
    dueDate: Date = new Date()
    numberInstallments: string | null = '1'
    typeInstallment?: string | null  = "Em Meses"
    daysBetweenInstallments?: number | null = null
    expenseType: string = ""; 

    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()
    idUser: number = 0;
    idAccount: number = 0;
    idExpenseCategory: number = 0;
    idPaymentMethods: number = 0;
    idTypePerson: number = 0;

    expenseCategory: IExpenseCategory = new IExpenseCategory
    account:  IAccount = new IAccount
    supplier: ITypePerson = new ITypePerson
    paymentMethods: IPaymentMethods | null = new IPaymentMethods

    expenseInstallments = [] as IExpenseInstallments[]
    //installments
    idInstallment: number = 0
    installmentDescription: string = ""
    valueInstallment: number = 0
    isPaid: boolean = false


}

export class IExpenseInstallments{
    id:  number = 0
    idExpense: number = 0
    dueDate: Date = new Date()
    installment: number = 0
    value: number = 0
    isPaid: boolean = false
    paymentDate: Date | null = null

    dueDateFormatter: string = ""
    installmentFormatter: string = ""
    valueFormatter: string = ""

}

