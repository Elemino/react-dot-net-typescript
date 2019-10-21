import * as React from 'react';
import BudgetService, { IBudget, departments } from '../services/Budgets';
import { RouteComponentProps } from 'react-router';
import { VictoryBar, VictoryPie } from 'victory';
import { subMonths, subYears, startOfQuarter, endOfQuarter, subQuarters } from 'date-fns';
let budgetService = new BudgetService();
const dateRangeOptions = [
    { value: 'yearToDate', label: 'Year to Date' },
    { value: 'quarterToDate', label: 'Quarter to Date' },
    { value: 'lastCalendarYear', label: 'Last Calendar Year' },
    { value: 'lastQuarter', label: 'Last Quarter' },
    { value: 'lastMonth', label: 'Last Month' },
];
const getDateRange = (type: String): [number, number, number, number] => {
    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    switch (type) {
        case 'yearToDate': {
            return [date.getMonth() + 1, year, month, year]
        }
        case 'quarterToDate': {
            const start = startOfQuarter(date);
            return [start.getMonth() + 1, start.getFullYear(), month, year]
        }
        case 'lastCalendarYear': {
            const calYear = subYears(date, 1);
            return [1, calYear.getFullYear(), 12, calYear.getFullYear()]
        }
        case 'lastQuarter': {
            const start = startOfQuarter(subQuarters(date, 1));
            const end = endOfQuarter(subQuarters(date, 1));
            return [start.getMonth() + 1, start.getFullYear(), end.getMonth() + 1, end.getFullYear()]
        }
        case 'lastMonth': {
            const start = subMonths(date, 1);
            const end = subMonths(date, 1);
            return [start.getMonth() + 1, start.getFullYear(), end.getMonth() + 1, end.getFullYear()]
        }
        case 'last12Months': {
            const start = subMonths(date, 12);
            const end = subMonths(date, 1);
            return [start.getMonth() + 1, start.getFullYear(), end.getMonth() + 1, end.getFullYear()]
        }
        default:
            return [1, year, 1, year]
    }
};
const departmentOptions = [
    { value: '', label: 'All' },
    ...Array.from(departments).map(([value, label]) => ({
        value, label
    }))
]
export class Contacts extends React.Component<RouteComponentProps<any>, any> {
    state = {
        budgetsByDepartmentPeriod: 'yearToDate',
        budgetsByDepartment: {} as any,
        expensesByDepartmentPeriod: 'yearToDate',
        expensesByDepartment: {} as any,
        bugdetMonthByMonthDepartment: '',
        bugdetMonthByMonth: {} as any,
        expensesMonthByMonthDepartment: '',
        expensesMonthByMonth: {} as any
    };
    componentDidMount() {
        this.loadData();
    }
    loadData = async () => {
        const {
            budgetsByDepartmentPeriod,
            expensesByDepartmentPeriod,
            bugdetMonthByMonthDepartment,
            expensesMonthByMonthDepartment,
        } = this.state;
        const [
            budgetsByDepartmentData,
            expensesByDepartmentData,
            bugdetMonthByMonthData,
            expensesMonthByMonthData
        ] = await Promise.all([
            budgetService.budgetAndExpensesOverPeriod(...getDateRange(budgetsByDepartmentPeriod)),
            budgetService.budgetAndExpensesOverPeriod(...getDateRange(expensesByDepartmentPeriod)),
            budgetService.budgetAndExpensesOverPeriod(...getDateRange('last12Months')),
            budgetService.budgetAndExpensesOverPeriod(...getDateRange('last12Months')),
        ]);
        const budgetsByDepartment = budgetsByDepartmentData.content.reduce<any>((acc, cur) => {
            acc[cur.department] = acc[cur.department] || 0;
            acc[cur.department] += cur.amountBudgeted;
            return acc;
        }, {});
        const expensesByDepartment = expensesByDepartmentData.content.reduce<any>((acc, cur) => {
            acc[cur.department] = acc[cur.department] || 0;
            acc[cur.department] += cur.amountSpent;
            return acc;
        }, {});
        const bugdetMonthByMonth = bugdetMonthByMonthData.content
            .filter(budget =>
                !bugdetMonthByMonthDepartment || budget.department.toString() === bugdetMonthByMonthDepartment)
            .reduce<any>((acc, cur) => {
                acc[cur.calendarMonth] = acc[cur.calendarMonth] || 0;
                acc[cur.calendarMonth] += cur.amountBudgeted;
                return acc;
            }, {});
        const expensesMonthByMonth = expensesMonthByMonthData.content
            .filter(budget =>
                !expensesMonthByMonthDepartment || budget.department.toString() === expensesMonthByMonthDepartment)
            .reduce<any>((acc, cur) => {
                acc[cur.calendarMonth] = acc[cur.calendarMonth] || 0;
            acc[cur.calendarMonth] += cur.amountSpent;
                acc[cur.calendarMonth] += cur.amountBudgeted;
                return acc;
            }, {});
        this.setState({
            searchQuery: '',
            budgetsByDepartment,
            expensesByDepartment,
            bugdetMonthByMonth,
            expensesMonthByMonth
        });
    }
    handleChange = (field: string) => (event: React.ChangeEvent<HTMLSelectElement>) => {
        this.setState({ [field]: event.target.value }, this.loadData);
    }
    render() {
        const {
            budgetsByDepartmentPeriod,
            budgetsByDepartment,
            expensesByDepartmentPeriod,
            expensesByDepartment,
            bugdetMonthByMonthDepartment,
            bugdetMonthByMonth,
            expensesMonthByMonthDepartment,
            expensesMonthByMonth,
        } = this.state;
        return (
            <div style={{ maxWidth: 500, margin: '0 auto' }}>
                <h2>Calendar Year to Date Budget by Department</h2>
                <select value={budgetsByDepartmentPeriod} onChange={this.handleChange('budgetsByDepartmentPeriod')}>
                    {dateRangeOptions.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                </select>
                <VictoryPie
                    data={Object.keys(budgetsByDepartment).map(department => ({
                        x: departments.get(department),
                        y: budgetsByDepartment[department]
                    }))}
                />
                <h2>Calendar Year to Date Expense by Department</h2>
                <select value={expensesByDepartmentPeriod} onChange={this.handleChange('expensesByDepartmentPeriod')}>
                    {dateRangeOptions.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                </select>
                <VictoryPie
                    data={Object.keys(expensesByDepartment).map(department => ({
                        x: departments.get(department),
                        y: expensesByDepartment[department]
                    }))}
                />
                <h2>Month by Month Budget over last 12 months</h2>
                <select value={bugdetMonthByMonthDepartment} onChange={this.handleChange('bugdetMonthByMonthDepartment')}>
                    {departmentOptions.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                </select>
                <VictoryBar
                    data={Object.keys(bugdetMonthByMonth).map(month => ({
                        label: month,
                        x: month,
                        y: bugdetMonthByMonth[month]
                    }))}
                />
                <h2>Month by Month Expenses over last 12 month</h2>
                <select value={expensesMonthByMonthDepartment} onChange={this.handleChange('expensesMonthByMonthDepartment')}>
                    {departmentOptions.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                </select>
                <VictoryBar
                    data={Object.keys(expensesMonthByMonth).map(month => ({
                        label: month,
                        x: month,
                        y: expensesMonthByMonth[month]
                    }))}
                />
            </div>
        );
    }
}