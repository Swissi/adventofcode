require 'date'
lines = File.readlines('i.txt')
logs = {}
guards = {}
sleepy_minutes = {}
last_guard = ''
start_minute = 0

# sort input file
lines.each do |line|
  date_part = DateTime.parse(line[1..16])
  logs[date_part] = line[19..line.length]
end

logs.sort.each do |log|
  msg = log[1]
  # if # is in string a guard is registered
  if msg.include? '#'
    id = msg.split(' ')[1].split('#')[1]
    last_guard = Integer(id)

    guards[last_guard] = {} if guards[last_guard].nil?
  end

  # log the minute the guard fell asleep
  start_minute = log[0].min if msg.include? 'falls asleep'

  # if he wakes up make calculations
  next unless msg.include? 'wakes up'

  # register all minutes where he fell asleep
  (start_minute..(log[0].min - 1)).each do |i|
    if sleepy_minutes[i].nil?
      sleepy_minutes[i] = 0
    else
      sleepy_minutes[i] += 1
    end

    if guards[last_guard][i].nil?
      guards[last_guard][i] = 1
    else
      guards[last_guard][i] += 1
    end
  end
end

most_times = 0
most_guard = 0
most_minute = 0
guards.each do |g|
  m = g[1].max_by { |_, y| y }

  next if m.nil?
  next unless m[1] > most_times

  most_times = m[1]
  most_minute = m[0]
  most_guard = g[0]
end

puts most_minute * Integer(most_guard)
