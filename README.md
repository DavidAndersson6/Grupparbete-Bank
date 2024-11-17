<img width="261" alt="FlammanBank" src="https://github.com/user-attachments/assets/64210aac-7fa9-4c27-9d95-18a62871895a">

# FlammanBank
This is a bank application built in C# and utilizes object-oriented programming to offer users a range of banking services. The application enable users to create an account, open and manage savings accounts, track balances and activity across accounts and view account details.
The application is designed to provide a straightforward and user-friendly experience for securely and efficiently managing finances and accounts.

## Table of contents
* [FlammanBank](#introduction)  
  
* [Features](#features)  
  
* [Getting started](#gettingStarted)

* [Code structure](#codeStructure) 

* [Usage](#usage)

* [Diagrams](#diagrams)
  
* [The Team](#theTeam)


## Features
- #### Account Overview and Management:    
Users can view their account balances, transfer funds internally and externally, and open new accounts, including foreign currency accounts. 
- #### Savings and Loans:    
Users can open savings accounts with interest calculations and apply for loans with limits based on their current holdings.  
- #### Transaction Logs and Scheduling:    
Users can view detailed transaction logs, while transactions are processed every 15 minutes for better control.  


## Getting Started

### Prerequisites
Before you start, make sure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet) (compatible with the version used in this project  
- A code editor, e.g., Visual Studio.

### Installation
Step 1: Clone the Project
Clone the project repository from GitHub to your local machine with the following commands:
```bash
git clone https://github.com/DavidAndersson6/Grupparbete-Bank.git
```
Step 2: Navigate to the project dictionary
```bash
cd Grupparbete-Bank
```

## Code structure
### - Class 1: BankApp.cs   
Represents the bank's core functionality, managing users, accounts, transactions, and exchange rates. It also enforces security measures, such as locking accounts after multiple failed login attempts.  
  
#### Methods:  

SetDefaultExchangeRates(): Sets default exchange rates for common currency pairs (e.g., SEK → USD, USD → SEK).  
RegisterUser(): Registers a new user. If the username exists, an error message is shown. Customers can select a preferred currency, and a new account is created in that currency or the default (SEK).  
Login(): Allows administrators to create new users. Validates permissions before calling RegisterUser().  
UpdateExchangeRate(): Updates the exchange rate for a specific currency pair.  
ConvertCurrency(): Converts an amount between two currencies based on exchange rates. Skips conversion if no rate exists for the pair.  
TransferToOtherUser(): Manages transfers between accounts. Verifies the sender's balance and searches for the recipient's account. Converts currencies if necessary using ConvertCurrency().  
#### Purpose:  
This class serves as the backbone of the banking system, handling operations like user registration, currency management, and inter-account transfers. It ensures accurate and secure financial operations through validation and conversion logic.  

### - Class 2: Bank.CS  
Manages users, accounts, transactions, and exchange rates, with built-in security features like account locking.  
  
#### Methods:  
  
SetDefaultExchangeRates(): Initializes standard exchange rates for common currency pairs.  
RegisterUser(): Registers new users and creates accounts in a selected or default currency.  
Login(): Validates admin permissions and allows user creation via RegisterUser().  
UpdateExchangeRate(): Updates specific currency pair exchange rates.  
ConvertCurrency(): Converts amounts between currencies using existing exchange rates.  
TransferToOtherUser(): Executes user-to-user transfers, including currency conversion if needed.  
  
#### Purpose:  
Core functionality for user and account management, currency handling, and secure transactions.  

### - Class 3: BankAccount.cs  
Manages bank accounts, handling basic operations like deposits and withdrawals while ensuring transaction validity.  

#### Methods:  

Deposit(decimal amount): Deposits a positive amount into the account, increasing the balance.  
Withdraw(decimal amount): Withdraws a positive amount if the balance is sufficient. Returns true if successful, false if not.  
  
#### Purpose:  
Handles account transactions, ensuring valid deposits and withdrawals for different account and currency types.  

### - Class 4: UserObjects.cs  
Defines user roles, properties, and methods for managing users and their accounts. Implements account security with failed login attempts, account locking, and unlocking mechanisms.  
  
#### Methods:  

CheckPassword(string password): Verifies if the provided password matches the user’s password.  
AddFailedAttempts(): Increases the count of failed login attempts and locks the account after three failed attempts.  
ResetFailedAttempts(): Resets the failed login attempt counter.  
LockAccount(): Locks the user’s account for a specified duration.
UnlockAccount(): Unlocks the user’s account and resets the lock status.  
AddAccount(BankAccount account): Associates a new bank account with the user.  
TransferBetweenAccount(string fromAccountNumber, string toAccountNumber, decimal amount): Transfers funds between two accounts owned by the user, verifying balance and account existence.  
  
#### Purpose:  
This class manages user details, including roles, account security, and associated bank accounts. It ensures secure login attempts and supports basic operations like transferring funds between a user’s accounts.

# Diagrams

## Class diagram
<img width="680" alt="ClassDiagram1" src="https://github.com/user-attachments/assets/a8f3035c-4f9b-4e04-ad2d-0668e8108387">


## Use case diagram
![Use case diagram](https://github.com/user-attachments/assets/70589340-55c9-46ae-9e30-144c8a6fcedd)

## The Team
David Andersson - [DavidAndersson6](https://github.com/DavidAndersson6)  
  
Alfred Ochieng Osewe Okoth - [alfrokot100](https://github.com/alfrokot100)  
  
Henric Kurtsson - [Trucksson](https://github.com/Trucksson)  
  
Hanna Mikho - [hmikho](https://github.com/hmikho) 











