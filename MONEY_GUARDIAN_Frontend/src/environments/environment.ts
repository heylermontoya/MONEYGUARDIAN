import { api } from "./api.constants";
import { YARP_CEIBA_XM } from "./environment.constants";

export const environment = {
    production: false,
    endpoint_api_deposit: `${YARP_CEIBA_XM}${api.deposit}`,
    endpoint_api_user: `${YARP_CEIBA_XM}${api.user}`,
    endpoint_view_transaction: `${YARP_CEIBA_XM}${api.ViewTransaction}`,
    endpoint_api_MonetaryFund: `${YARP_CEIBA_XM}${api.MonetaryFund}`,
    endpoint_api_ExpenseType: `${YARP_CEIBA_XM}${api.ExpenseType}`,
    endpoint_api_Expense: `${YARP_CEIBA_XM}${api.Expense}`,
    endpoint_api_Budget: `${YARP_CEIBA_XM}${api.Budget}`,

    firebaseConfig: {
        apiKey: "AIzaSyD8HUYxexZO1dM7iKAZ-K1qUzHOxrcVZLg",
        authDomain: "loguin-3ae79.firebaseapp.com",
        projectId: "loguin-3ae79",
        storageBucket: "loguin-3ae79.appspot.com",
        messagingSenderId: "763809996450",
        appId: "1:763809996450:web:56bbf83cae14ae87acc1fb",
        measurementId: "G-RZJ10SMTB3"
    }
};
