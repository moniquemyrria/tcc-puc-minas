export class IAccountCategory {
    id: number = 0
    initials: string = ""
    description: string = ""
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: Date = new Date()
    idUser: number = 0;
}

