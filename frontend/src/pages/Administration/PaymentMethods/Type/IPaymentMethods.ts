
import { IAccount } from "../../Account/Type/IAccount"
import { IPaymentMethodsTypes } from "./IPaymentMethodsTypes"

export class IPaymentMethods {
    id: number = 0
    name: string = ""
    acceptInstallment: boolean = true
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()
    idUser: number = 0;
    idAccount: number = 0;

    account:  IAccount = new IAccount
    paymentMethodsTypes: IPaymentMethodsTypes[] = [] as IPaymentMethodsTypes[]
}

