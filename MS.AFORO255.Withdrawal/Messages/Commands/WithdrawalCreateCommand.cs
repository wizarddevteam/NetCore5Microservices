﻿using Aforo255.Cross.Event.Src.Commands;

namespace MS.AFORO255.Withdrawal.Messages.Commands
{
    public class WithdrawalCreateCommand : Command
    {
        public int IdTransaction { get; protected set; }
        public decimal Amount { get; protected set; }
        public string Type { get; protected set; }
        public string CreationDate { get; protected set; }
        public int AccountId { get; protected set; }

        public WithdrawalCreateCommand(int idTransaction, decimal amount, string type, string creationDate, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            CreationDate = creationDate;
            AccountId = accountId;
        }
    }
}
