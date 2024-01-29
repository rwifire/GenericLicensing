using DDDBase.Models;

namespace GenericLicensing.Infrastructure.Tests;

public class MockEvent : BaseDomainEvent
{
  public string WhatsDone { get; }

  private MockEvent()
  {
  }

  public MockEvent(Guid dId, string whatsDone, long version) : base(dId, version)
  {
    WhatsDone = whatsDone;
  }
}