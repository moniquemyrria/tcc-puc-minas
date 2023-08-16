

export class IUsers {

    //UserDTO
    id: string = ""
    isActive: Boolean = true
    dateCreated: Date = new Date()
    dateUpdate?: null | Date = null


    //IdentityUser
    name: string = ""
    userName: string = ""
    normalizedUserName: string = ""
    email: string = ""
    normalizedEmail: string = ""
    emailConfirmed: Boolean = true
    passwordHash: string = ""
    securityStamp: string = ""
    concurrencyStamp: string = ""
    phoneNumber: string = ""
    phoneNumberConfirmed: Boolean = false
    twoFactorEnabled: Boolean = false
    lockoutEnd: Date = new Date()
    lockoutEnabled: Boolean = false
    accessFailedCount: number = 0
    passwordRegister: string = ""
    
}

