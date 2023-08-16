import { IAccount } from "@/pages/Administration/Account/Type/IAccount"
import { IPaymentMethods } from "@/pages/Administration/PaymentMethods/Type/IPaymentMethods"


export class IExpensePayment {
    
    idInstallment: number = 0
    idExpense: number = 0
    amount: number = 0
    paymentDate: Date = new Date()
    account:  IAccount = new IAccount
    paymentMethods: IPaymentMethods = new IPaymentMethods
}

