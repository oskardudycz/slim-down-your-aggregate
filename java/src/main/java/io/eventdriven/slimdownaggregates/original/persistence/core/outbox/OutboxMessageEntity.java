package io.eventdriven.slimdownaggregates.original.persistence.core.outbox;

import io.eventdriven.slimdownaggregates.original.infrastructure.events.EventEnvelope;
import jakarta.persistence.*;

import java.time.OffsetDateTime;
import java.util.UUID;
import com.fasterxml.jackson.databind.ObjectMapper;

@Entity
@Table(name = "outboxmessages")
public class OutboxMessageEntity {
  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Long position;

  @Column(nullable = false)
  private String messageId;

  @Column(nullable = false)
  private String messageType;

  @Column(nullable = false)
  private String data;

  @Column(nullable = false)
  private OffsetDateTime scheduled;

  // Default constructor required by JPA
  public OutboxMessageEntity() {}

  public OutboxMessageEntity(String messageId, String messageType, String data, OffsetDateTime scheduled) {
    this.messageId = messageId;
    this.messageType = messageType;
    this.data = data;
    this.scheduled = scheduled;
  }

  public static OutboxMessageEntity from(EventEnvelope eventEnvelope, ObjectMapper objectMapper) {
    try {
      String serializedData = objectMapper.writeValueAsString(eventEnvelope.event());

      return new OutboxMessageEntity(
        UUID.randomUUID().toString(),
        eventEnvelope.event().getClass().getName(),
        serializedData,
        OffsetDateTime.now()
      );
    } catch (Exception ex) {
      throw new RuntimeException("Error serializing eventEnvelope", ex);
    }
  }
}

