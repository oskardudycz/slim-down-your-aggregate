package io.eventdriven.slimdownaggregates.original.user.core;

public final class AppUserConstants {

  private AppUserConstants() {

  }

  public static final String PASSWORD_NEVER_EXPIRES = "passwordNeverExpires";
  public static final String IS_SELF_SERVICE_USER = "isSelfServiceUser";
  public static final String CLIENTS = "clients";

  // TODO: Remove hard coding of system user name and make this a configurable parameter
  public static final String SYSTEM_USER_NAME = "system";
}
