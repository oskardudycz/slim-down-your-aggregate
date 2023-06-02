package io.eventdriven.slimdownaggregates.original.user.core;

public class JsonCommand {
  public String stringValueOfParameterNamed(String username) {
    return null;
  }

  public Boolean booleanObjectValueOfParameterNamed(String sendPasswordToEmail) {
    return null;
  }

  public boolean parameterExists(String passwordNeverExpires) {
    return false;
  }

  public boolean booleanPrimitiveValueOfParameterNamed(String passwordNeverExpires) {
      return false;
  }

  public boolean hasParameter(String passwordParamName) {
    return false;
  }

  public boolean isChangeInPasswordParameterNamed(String passwordParamName, String password, PlatformPasswordEncoder platformPasswordEncoder, Long id) {
    return false;
  }

  public String passwordValueOfParameterNamed(String passwordParamName, PlatformPasswordEncoder platformPasswordEncoder, Long id) {
    return null;
  }

  public boolean isChangeInStringParameterNamed(String passwordEncodedParamName, String password) {
    return false;
  }

  public Long longValueOfParameterNamed(String officeIdParamName) {
    return null;
  }

  public boolean isChangeInLongParameterNamed(String officeIdParamName, Long id) {
      return false;
  }

  public boolean isChangeInArrayParameterNamed(String rolesParamName, String[] rolesAsIdStringArray) {
    return false;
  }

  public String[] arrayValueOfParameterNamed(String rolesParamName) {
    return new String[0];
  }

  public boolean isChangeInBooleanParameterNamed(String passwordNeverExpire, boolean passwordNeverExpires) {
    return false;
  }
}
