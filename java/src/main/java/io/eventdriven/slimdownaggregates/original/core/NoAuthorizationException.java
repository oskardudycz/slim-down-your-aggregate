package io.eventdriven.slimdownaggregates.original.core;

public class NoAuthorizationException extends RuntimeException {

  public NoAuthorizationException(final String message) {
    super(message);
  }
}
