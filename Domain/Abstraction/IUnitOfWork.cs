﻿namespace Domain.Abstraction;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
