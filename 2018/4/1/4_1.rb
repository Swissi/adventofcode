# [1518-05-08 00:12] falls asleep
# [1518-09-09 00:04] Guard #1543 begins shift
# [1518-04-05 00:00] Guard #131 begins shift
# [1518-09-12 00:54] falls asleep
# [1518-08-28 00:12] falls asleep
# [1518-06-06 00:25] wakes up

require 'date'
lines = File.readlines('input.txt')
logs = {}
guards = {}
guards_total = {}
last_guard = ''
start_minute = 0

lines.each do |line|
  date_part = DateTime.parse(line[1..16])
  logs[date_part] = line[19..line.length]
end

logs.sort.each do |log|
  msg = log[1]

  if msg.include? '#'
    id = msg.split(' ')[1].split('#')[1]
    last_guard = Integer(id)

    guards[last_guard] = {} if guards[last_guard].nil?

    guards_total[last_guard] = 0 if guards_total[last_guard].nil?

  end

  start_minute = log[0].min if msg.include? 'falls asleep'

  next unless msg.include? 'wakes up'

  guards[last_guard] = {} if guards[last_guard].nil?

  (start_minute..log[0].min-1).each do |i|
    if guards[last_guard][i].nil?
      guards[last_guard][i] = 1
    else
      guards[last_guard][i] += 1
    end
    guards_total[last_guard] += 1
  end

end

#get guard who slept the most
sleepy_guard = guards_total.sort_by{|x,y| y}.reverse.first[0]

#get minute where he sleeps most
sleepy_minutes_by_guard = guards[sleepy_guard]
sleepy_minute = sleepy_minutes_by_guard.sort_by{|x,y| y}.reverse.first[0]

strat1 = sleepy_guard * sleepy_minute
puts strat1
