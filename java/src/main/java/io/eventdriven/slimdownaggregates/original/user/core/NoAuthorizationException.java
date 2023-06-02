package io.eventdriven.slimdownaggregates.original.user.core;

public class NoAuthorizationException extends RuntimeException {

  public NoAuthorizationException(final String message) {
    super(message);
  }
}
