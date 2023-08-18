import { createRouter, createWebHistory } from "vue-router";

const base = import.meta.env.BASE_URL;

const routes = createRouter({
  history: createWebHistory(base),
  routes: [
    {
      path: "/",
      name: "Login",
      component: () => import("@pages/Login/Login.vue"),
    },

    {
      path: "/refreshPassword",
      name: "Refresh Password",
      component: () => import("@pages/RefreshPassword/RefreshPassword.vue"),
    },
    {
      path: "/home",
      name: "KMenusToolbar",
      component: () => import("../components/KMenusToolbar.vue"),

      children: [
        //Dashboard
        {
          path: "/dashboard",
          name: "Dashboard Form",
          component: () => import("@pages/Dashboard/Dashboard.vue"),
        },

        //Product
        {
          path: "/listProducts",
          name: "List Products",
          component: () => import("@pages/Products/Products/ListProducts.vue"),
        },

        //Expense Category
        {
          path: "/listExpenseCategory",
          name: "List Expense Category",
          component: () =>
            import(
              "@pages/Administration/ExpenseCategory/ListExpenseCategory.vue"
            ),
        },
        {
          path: "/formExpenseCategory",
          name: "Form Expense Category",
          component: () =>
            import(
              "@pages/Administration/ExpenseCategory/FormExpenseCategory.vue"
            ),
        },
        {
          path: "/editExpenseCategory/:id",
          name: "Edit Expense Category",
          component: () =>
            import(
              "@pages/Administration/ExpenseCategory/FormExpenseCategory.vue"
            ),
        },

        //Revenue Category
        {
          path: "/listRevenueCategory",
          name: "List Revenue Category",
          component: () =>
            import(
              "@pages/Administration/RevenueCategory/ListRevenueCategory.vue"
            ),
        },
        {
          path: "/formRevenueCategory",
          name: "Form Revenue Category",
          component: () =>
            import(
              "@pages/Administration/RevenueCategory/FormRevenueCategory.vue"
            ),
        },
        {
          path: "/editRevenueCategory/:id",
          name: "Edit Revenue Category",
          component: () =>
            import(
              "@pages/Administration/RevenueCategory/FormRevenueCategory.vue"
            ),
        },

        //Account Category
        {
          path: "/listAccountCategory",
          name: "List Account Category",
          component: () =>
            import(
              "@pages/Administration/AccountCategory/ListAccountCategory.vue"
            ),
        },
        {
          path: "/formAccountCategory",
          name: "Form Account Category",
          component: () =>
            import(
              "@pages/Administration/AccountCategory/FormAccountCategory.vue"
            ),
        },
        {
          path: "/editAccountCategory/:id",
          name: "Edit Account Category",
          component: () =>
            import(
              "@pages/Administration/AccountCategory/FormAccountCategory.vue"
            ),
        },

        //Type Person
        {
          path: "/listTypePerson",
          name: "List Type Person",
          component: () =>
            import("@pages/Administration/TypePerson/ListTypePerson.vue"),
        },
        {
          path: "/formTypePerson",
          name: "Form Type Person",
          component: () =>
            import("@pages/Administration/TypePerson/FormTypePerson.vue"),
        },
        {
          path: "/editTypePerson/:id",
          name: "Edit Type Person",
          component: () =>
            import("@pages/Administration/TypePerson/FormTypePerson.vue"),
        },

        //Account
        {
          path: "/listAccount",
          name: "List Account",
          component: () =>
            import("@pages/Administration/Account/ListAccount.vue"),
        },
        {
          path: "/formAccount",
          name: "Form Account",
          component: () =>
            import("@pages/Administration/Account/FormAccount.vue"),
        },
        {
          path: "/editAccount/:id",
          name: "Edit Account",
          component: () =>
            import("@pages/Administration/Account/FormAccount.vue"),
        },

        //Payment Methods
        {
          path: "/listPaymentMethods",
          name: "List Payment Methods",
          component: () =>
            import(
              "@pages/Administration/PaymentMethods/ListPaymentMethods.vue"
            ),
        },
        {
          path: "/formPaymentMethods",
          name: "Form Payment Methods",
          component: () =>
            import(
              "@pages/Administration/PaymentMethods/FormPaymentMethods.vue"
            ),
        },
        {
          path: "/editPaymentMethods/:id",
          name: "Edit Payment Methods",
          component: () =>
            import(
              "@pages/Administration/PaymentMethods/FormPaymentMethods.vue"
            ),
        },

        //Revenue
        {
          path: "/listRevenue",
          name: "List Revenue ",
          component: () => import("@pages/Revenue/ListRevenue.vue"),
        },
        {
          path: "/formRevenue",
          name: "Form Revenue ",
          component: () => import("@pages/Revenue/FormRevenue.vue"),
        },
        {
          path: "/editRevenue/:id",
          name: "Edit Revenue ",
          component: () => import("@pages/Revenue/FormRevenue.vue"),
        },

        //Expense
        {
          path: "/listExpense",
          name: "List Expense ",
          component: () => import("@pages/Expense/ListExpense.vue"),
        },

        // Expense Fixed
        {
          path: "/formExpenseFixed",
          name: "Form Expense Fixed",
          component: () => import("@pages/Expense/FormExpenseFixed.vue"),
        },
        {
          path: "/editExpenseFixed/:id",
          name: "Edit Expense Fixed",
          component: () => import("@pages/Expense/FormExpenseFixed.vue"),
        },

        // Expense Variable
        {
          path: "/formExpenseVariable",
          name: "Form Expense Variable",
          component: () => import("@pages/Expense/FormExpenseVariable.vue"),
        },
        {
          path: "/editExpenseVariable/:id",
          name: "Edit Expense Variable",
          component: () => import("@pages/Expense/FormExpenseVariable.vue"),
        },

        //users
        {
          path: "/listUsers",
          name: "List Users",
          component: () => import("@pages/Administration/Users/ListUsers.vue"),
        },
        {
          path: "/formUsers",
          name: "Form Users",
          component: () => import("@pages/Administration/Users/FormUsers.vue"),
        },
        {
          path: "/editUsers/:id",
          name: "Edit Users",
          component: () => import("@pages/Administration/Users/FormUsers.vue"),
        },
      ],
    },
  ],
});

export default routes;
