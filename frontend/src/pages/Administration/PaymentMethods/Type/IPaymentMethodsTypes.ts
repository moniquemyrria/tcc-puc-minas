

export class IPaymentMethodsTypes {
    id: number = 0
    description: string = ""
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()
    idPaymentMethods: number = 0;

    //valores das despesas
    value: number = 0
    trafficTicket: number = 0
    taxRate: number = 0
    totalPayment: number | null = 0
}

