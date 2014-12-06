(function () {
    'use strict';
    
    angular.module('vabank.webapp').constant('paymentsTree', {
        code: 'PAYMENT',
        name: 'Платежи',
        level: 0,
        children: [
            {
                code: 'PAYMENT-CUSTOM',
                name: 'Произвольные платежи',
                level: 1,
                children: [
                    {
                        level: 2,
                        code: 'PAYMENT-CUSTOM-PAYMENTORDER',
                        name: 'Платеж по реквизитам'
                    }
                ]
            },
            {
                code: 'PAYMENT-CELL',
                name: 'Мобильная связь',
                level: 1,
                children: [
                    {
                        code: 'PAYMENT-CELL-VELCOM',
                        name: 'Velcom',
                        level: 2,
                        children: [
                            {
                                level: 3,
                                code: 'PAYMENT-CELL-VELCOM-PHONENO',
                                name: 'По номеру телефона'
                            }
                        ]
                    }
                ]
            }//,
            //{
            //    code: 'PAYMENT-TEST',
            //    name: 'Тестовые платежи',
            //    level: 1,
            //    children: [
            //        {
            //            code: 'PAYMENT-TEST-L2',
            //            name: 'L2',
            //            level: 2,
            //            children: [
            //                {
            //                    level: 3,
            //                    code: 'PAYMENT-TEST-L2-L31',
            //                    name: 'L31',
            //                    children: [
            //                        {
            //                            level: 4,
            //                            code: 'PAYMENT-TEST-L2-L31-L4',
            //                            name: 'L41'
            //                        }
            //                    ]
            //                },
            //                {
            //                    level: 3,
            //                    code: 'PAYMENT-TEST-L2-L32',
            //                    name: 'L32',
            //                    children: [
            //                        {
            //                            level: 4,
            //                            code: 'PAYMENT-TEST-L2-L32-L4',
            //                            name: 'L42'
            //                        }
            //                    ]
            //                }
            //            ]
            //        }
            //    ]
            //},
        ]
    });
})();