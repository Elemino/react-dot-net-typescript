import RestUtilities from './RestUtilities';

export interface IBudget {
    id: number,
    calendarMonth: number;
    calendarYear: number;
    department: number;
    amountSpent: number;
    amountBudgeted: number;
}

export const departments = new Map([
    ['1', 'Products'],
    ['2', 'Marketing'],
    ['3', 'Sales'],
    ['4', 'Services'],
])

export default class Budgets {

    calendarYearBudgetAndExpenseByDepartment(year: number, department: number) {
        return RestUtilities.get<Array<IBudget>>(`/api/budget?year=${year}&department=${department}`);
    }

    // 1. Last Calendar Year
    // 2. Calendar year to date
    budgetAndExpensesByYear(year: number){
        return RestUtilities.get<Array<IBudget>>(`/api/budget?year=${year}`);
    }

    // Last Month
    bugetAndExpensesByMonthAndYear(month: number, year: number){
        return RestUtilities.get<Array<IBudget>>(`/api/budget?year=${year}&month=${month}`);
    }

    // This call can be used for the following queries:
    // 1. Calendar Year to Date
    // 2. Quarter to Date
    // 3. Last Quarter
    // 4. Month by Month expenses
    budgetAndExpensesOverPeriod(startMonth: number, startYear: number, endMonth: number, endYear: number, department: any = ''){
        return RestUtilities.get<Array<IBudget>>(`/api/budget?startMonth=${startMonth}&startYear=${startYear}&endMonth=${endMonth}&endYear=${endYear}&department=${department}`);
    }
}