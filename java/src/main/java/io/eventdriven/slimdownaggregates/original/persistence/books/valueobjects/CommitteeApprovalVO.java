package io.eventdriven.slimdownaggregates.original.persistence.books.valueobjects;

import jakarta.persistence.Embeddable;

@Embeddable
public class CommitteeApprovalVO {

  private boolean isApproved;
  private String feedback;

  // Default constructor for JPA
  public CommitteeApprovalVO() {}

  public CommitteeApprovalVO(boolean isApproved, String feedback) {
    this.isApproved = isApproved;
    this.feedback = feedback;
  }

  // Getters and setters

  public boolean isApproved() {
    return isApproved;
  }

  public void setApproved(boolean isApproved) {
    this.isApproved = isApproved;
  }

  public String getFeedback() {
    return feedback;
  }

  public void setFeedback(String feedback) {
    this.feedback = feedback;
  }
}

