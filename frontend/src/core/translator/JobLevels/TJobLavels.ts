import { storeUser } from "@/core/store/userStore";

const userStore = storeUser();

export class TListJobLevels {
    language: string = userStore.getLanguage
    title: string = ""
    subtitle: string = ""
    buttonNew: string = ""
    buttonTitle: string = ""
    search: string = ""
    header: any = {}
    buttonEdit: string = ""
    buttonBlock: string = ""
    buttonUnlock: string = ""
    breadCrumbs: any = {}

    onInit() {
        this.language = userStore.getLanguage
        this.title = this.language == '"P"' ? 'Cadastro de Nível de Cargo' : this.language == '"E"' ? 'Position Level Registration' : 'Registro de nivel de posición'
        this.subtitle = this.language == '"P"' ? 'Gerencie o cadastro de Níveis de Cargo' : this.language == '"E"' ? 'Manages the Job Levels register' : 'Administrar el Registro de Niveles de Puesto'
        this.buttonNew = this.language == '"P"' ? 'Novo' : this.language == '"E"' ? 'New' : 'Nuevo'
        this.buttonTitle = this.language == '"P"' ? 'Novo Nível' : this.language == '"E"' ? 'New Level' : 'Nuevo nivel'
        this.search = this.language == '"P"' ? 'Pesquisar' : this.language == '"E"' ? 'Search' : 'Buscar'
        this.buttonEdit = this.language == '"P"' ? 'Editar Nível do Cargo' : this.language == '"E"' ? 'Edit Position Level' : 'Editar nivel de posición'
        this.buttonBlock = this.language == '"P"' ? 'Bloquear Nível do Cargo' : this.language == '"E"' ? 'Block Position Level' : 'Nivel de posición de bloque'
        this.buttonUnlock = this.language == '"P"' ? 'Desbloquear Nível do Cargo' : this.language == '"E"' ? 'Unlock Position Level' : 'Nivel de posición de desbloqueo'

        this.header = [
            {
                name: "isActive",
                align: "left",
                label: this.language == '"P"' || this.language == '"E"' ? 'Status' : 'Estado',
                field: "isActive",
                sortable: false,
            },
            {
                name: "id",
                align: "left",
                label: this.language == '"P"' ? 'Código' : this.language == '"E"' ? 'Code' : 'Código',
                field: "id",
                sortable: true,
            },
            {
                name: "description",
                align: "left",
                label: this.language == '"P"' ? 'Descrição' : this.language == '"E"' ? 'Description' : 'Descripción',
                field: "description",
                sortable: true,
            },
            {
                name: "approvalLevels",
                align: "left",
                label: this.language == '"P"' ? 'Níveis de Aprovação' : this.language == '"E"' ? 'Approval Levels' : 'Niveles de Aprobación',
                field: "approvalLevels",
                sortable: true,
            },

            {
                name: "actions",
                align: "center",
                label: this.language == '"P"' ? 'Ações' : this.language == '"E"' ? 'Actions' : 'Acción',
                field: "actions",
                sortable: false,
            },
        ]

        this.breadCrumbs = [
            {
                label: this.language == '"P"' ? 'Lista Níveis de Cargo' : this.language == '"E"' ? 'List of Position Levels' : 'Lista de niveles de posición',
                router: "/listJobLevels",
            },
            {
                label: this.language == '"P"' ? 'Novo Nível do Cargo' : this.language == '"E"' ? 'New Position Level' : 'Nuevo nivel de posición',
                router: "/formJobLevels",
            },
        ]
    }


    private static _instace: TListJobLevels | null = null


    static getInstance() {
        if (this._instace == null) {
            this._instace = new TListJobLevels()
        }

        if (this._instace.language != userStore.language) {
            this._instace.onInit()
        }

        return this._instace
    }

}