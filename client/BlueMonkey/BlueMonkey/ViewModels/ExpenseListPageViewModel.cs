﻿using BlueMonkey.Business;
using BlueMonkey.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueMonkey.ViewModels
{
    public class ExpenseListPageViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;
        IExpenseService _expenseService;

        private IEnumerable<Expense> _items;
        public IEnumerable<Expense> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                SetProperty(ref _selectedExpense, value);
            }
        }

        public DelegateCommand<Expense> AddExpenseCommand => new DelegateCommand<Expense>(AddExpense);

        public ExpenseListPageViewModel(INavigationService navigationService, IExpenseService expenseService)
        {
            _navigationService = navigationService;
            _expenseService = expenseService;          
        }

        private void AddExpense(Expense expense)
        {
            var p = new NavigationParameters();
            p.Add("expense", expense);

            _navigationService.NavigateAsync("AddExpensePage", p);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            Items = await _expenseService.GetExpenses();
        }
    }
}
