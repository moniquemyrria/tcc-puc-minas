import { IProductsPriceList } from "@/pages/Products/Products/Types/IProductsPriceList"

export class IProducts {
    id: number = 0
    code: string = ""
    description: string = ""
    um: string = ""
    group: string = ""
    groupDescription: string = ""
    type: string = ""
    blocked: string = ""
    blockedStatus: string = ""
    accountingAccount: string = ""
    barCode: string = ""
    weight: string = ""
    grossWeight: string = ""
    origem: string = ""
    desciptionOrigem: string = ""
    pdvValue: number = 0
    stock: number = 0
    empenho: number = 0
    stockSP: number = 0
    category: string = ""
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()

    productsPriceList = [] as IProductsPriceList[]
}
