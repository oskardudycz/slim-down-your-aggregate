package io.eventdriven.slimdownaggregates.book.entities;

public class CommitteeApproval {
  private final boolean isApproved;
  private final String feedback;

  public CommitteeApproval(boolean isApproved, String feedback) {
    this.isApproved = isApproved;
    this.feedback = feedback;
  }

  public boolean isApproved() {
    return isApproved;
  }

  public String getFeedback() {
    return feedback;
  }
}
