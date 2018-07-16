using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StackExchange.Redis;

namespace EngineerTest.Models.Data
{
    public class CryptoTrade
    {
        [Key]
        [Column(Order = 1)]
        public Guid RunId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int TradeNum { get; set; }
        public long TimeStamp { get; set; }
        public string BaseCurrency { get; set; }
        public string SubCurrency { get; set; }
        public string Exchange { get; set; }
        public decimal Amount { get; set; }
        public decimal Volume { get; set; }
    }
}