package io.eventdriven.ecommerce.core.validation;

import java.util.regex.Pattern;

public class Check {
  public static <T> void IsNotNull(T value, String fieldName){
    if(value == null){
      throw new IllegalArgumentException("%s cannot be null!".formatted(fieldName));
    }
  }

  public static void IsNotEmpty(String text, String fieldName){
    IsNotNull(text, fieldName);

    if(text.isBlank()){
      throw new IllegalArgumentException("%s cannot be blank!".formatted(fieldName));
    }
  }

  public static void MatchesRegexp(String text, Pattern pattern, String fieldName){
    IsNotEmpty(text, fieldName);

    var regexMatcher = pattern.matcher(text);

    if(!regexMatcher.find())
      throw new IllegalArgumentException("%s is in wrong format".formatted(fieldName));
  }
}
