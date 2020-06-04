﻿using TellDontAskKata.Entities;

namespace TellDontAskKata.Repository
{
    public interface IOrderRepository
    {
        void Save(Order order);
        Order GetById(int orderId);
    }
}