﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueMonkey.Business;
using BlueMonkey.Model;
using BlueMonkey.ViewModels;
using Moq;
using Prism.Navigation;
using Xunit;

namespace BlueMonkey.ViewModel.Tests
{
    public class ReportPageViewModelTest
    {
        [Fact]
        public void Constructor()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();
            editReport.Setup(m => m.Name).Returns("Name");
            editReport.Setup(m => m.Date).Returns(DateTime.MinValue);

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            Assert.NotNull(actual.Name);
            Assert.Equal("Name", actual.Name.Value);

            Assert.NotNull(actual.Date);
            Assert.Equal(DateTime.MinValue, actual.Date.Value);

            Assert.Null(actual.Expenses);

            Assert.NotNull(actual.InitializeCommand);
            Assert.True(actual.InitializeCommand.CanExecute());

            Assert.NotNull(actual.NavigateExpenseSelectionCommand);
            Assert.True(actual.NavigateExpenseSelectionCommand.CanExecute());

            Assert.NotNull(actual.SaveReportCommand);
            Assert.True(actual.SaveReportCommand.CanExecute());
        }

        [Fact]
        public void NameProperty()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            Assert.Null(actual.Name.Value);

            // Update model.
            editReport.Setup(m => m.Name).Returns("Name");
            editReport
                .Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Name"));

            Assert.Equal("Name", actual.Name.Value);

            // Update ViewModel.
            actual.Name.Value = "Update name";
            editReport
                .VerifySet(m => m.Name = "Update name", Times.Once);
        }

        [Fact]
        public void DateProperty()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            Assert.Equal(default(DateTime), actual.Date.Value);

            // Update model.
            editReport.Setup(m => m.Date).Returns(DateTime.MaxValue);
            editReport
                .Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Date"));

            Assert.Equal(DateTime.MaxValue, actual.Date.Value);

            // Update ViewModel.
            actual.Date.Value = default(DateTime);
            editReport
                .VerifySet(m => m.Date = default(DateTime), Times.Exactly(2));
        }

        [Fact]
        public void ExpensesProperty()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);


            var expenses = new SelectableExpense[] {};

            Assert.PropertyChanged(actual, "Expenses", () => { actual.Expenses = expenses; });

            Assert.Equal(expenses, actual.Expenses);
        }

        [Fact]
        public void InitializeCommand()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var expense01 = new SelectableExpense(new Expense()) { IsSelected = false };
            var expense02 = new SelectableExpense(new Expense()) { IsSelected = true };
            var expenses = new ObservableCollection<SelectableExpense>(new[] { expense01, expense02 });
            editReport
                .Setup(m => m.SelectableExpenses)
                .Returns(new ReadOnlyObservableCollection<SelectableExpense>(expenses));

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            Assert.Null(actual.Expenses);

            actual.InitializeCommand.Execute();

            Assert.NotNull(actual.Expenses);
            Assert.Equal(1, actual.Expenses.Count());
            Assert.Equal(expense02, actual.Expenses.First());
        }

        [Fact]
        public void NavigateExpenseSelectionCommand()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            actual.NavigateExpenseSelectionCommand.Execute();

            navigationService.Verify(m => m.NavigateAsync("ExpenseSelectionPage", null, null, true), Times.Once);
        }

        [Fact]
        public void SaveReportCommand()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            actual.SaveReportCommand.Execute();

            editReport.Verify(m => m.SaveAsync(), Times.Once);
            navigationService.Verify(m => m.GoBackAsync(null, null, true), Times.Once);
        }

        [Fact]
        public void OnNavigatedFrom()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            actual.OnNavigatedFrom(null);
        }

        [Fact]
        public void OnNavigatedToWhenNewCreation()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            var navigationParameter = new NavigationParameters();
            navigationParameter[ReportPageViewModel.ReportIdKey] = null;
            actual.OnNavigatedTo(navigationParameter);

            editReport.Verify(m => m.InitializeForNewReportAsync(), Times.Once);
        }

        [Fact]
        public void OnNavigatedToWhenUpdateReport()
        {
            var navigationService = new Mock<INavigationService>();
            var editReport = new Mock<IEditReport>();

            var actual = new ReportPageViewModel(navigationService.Object, editReport.Object);

            var navigationParameter = new NavigationParameters();
            navigationParameter[ReportPageViewModel.ReportIdKey] = "T.B.D";
            actual.OnNavigatedTo(navigationParameter);

            // TODO:
        }
    }
}
